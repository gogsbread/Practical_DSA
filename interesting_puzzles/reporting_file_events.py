'''
File Events
To keep all your computers in sync, Dropbox watches your file system for any changes within your Dropbox folder. Unfortunately, in many cases the file events received are too coarse to be presentable to non-savvy users, usually just an ADD event for any new file and a DEL event for any file that went missing.

You'll be given a list of ADD/DEL file events that should be processed and turned into a human-readable event feed that includes richer events like file and directory renames, moves, and copies.

Input
Your program must read an integer N (1 <= N <= 50000) from STDIN representing the number of file events in the test input, followed by that many file event rows. Each row will have the file event type, a UNIX timestamp of when the event occurred, the path of the file relative to the Dropbox root, and an 8-character hash of the contents (or former contents) of the file. Each row is separated by a space. No file paths include spaces. Directories are files too and may be empty (they'll have "-" as their file hash).

Output
Your output should be a series of English sentences to stdout, one per line, in some way describing the file events in a user-friendly manner. There is no objectively 'right' answer here, and in fact there may be multiple ways to interpret a provided list of file events. We'll be judging submissions on a number of criteria including raw efficiency, friendliness of output, ability to handle ambiguity, and more. As one example, the sample output below is (probably) the correct interpretation of the input file events, but is not particularly user-friendly. Feel free to deviate substantially from the sample.

Sample Input

6
ADD 1282352346 /test -
ADD 1282353016 /test/1.txt f2fa762f
DEL 1282354012 /test -
DEL 1282354012 /test/1.txt f2fa762f
ADD 1282354013 /test2 -
ADD 1282354013 /test2/1.txt f2fa762f
Sample Output

Added dir /test.
Added file /test/1.txt.
Renamed dir /test -> /test2.'''
#consider renmaing to filestatus
# there are 2 kinds of hashes : contenthash & object hash
    # object hash - identifies the object
    # contenthash - identifies the constituents. for directory, it would be the hash of objects inside that directory
    # dont store time
class File:
    def __init__(self,name,parentname,time,contenthash,ignore_create_action=False):
        self.created_time = time
        self.modified_time = time
        self.name = name
        self.parentname = parentname
        self.content_hash = contenthash
        self.events = []
        if not ignore_create_action:
            self.status = "c"
            self.events.append(("c",time,name,parentname,contenthash))
    def delete(self,time):
        self.status = "d"
        self.events.append(("d",time))
        self.modified_time = time
    def update(self,time,contenthash):
        self.status = "u"
        self.events.append(("u",time,self.content_hash,contenthash))
        self.content_hash = contenthash
        self.modified_time = time
    def rename(self,time,name):
        self.status = "r"
        self.events.append(("r",time,self.name,name))
        self.name = name
        self.modified_time = time
    def move(self,time,parentname):
        self.status = "m"
        self.events.append(("m",time,self.parentname,parentname))
        self.parentname = parentname
        self.modified_time = time
    def __hash__(self):
        return int(str(self.created_time) + str(name))

def _can_combine_transactions(transA,transB):
    '''
Renames,moves & updates occur at different times but it can be combined into one transaction
'''
    if len(transA) != len(transB):
        return False
    for i in range(len(transA)):
        actionA,timeA,nameA,contenthashA = eventA = transA[i].split(' ')
        actionB,timeB,nameB,contenthashB = eventB = transB[i].split(' ')
        filenameA = get_file_name(nameA)
        parentnameA = get_parent_name(nameA)
        filenameB = get_file_name(nameB)
        parentnameB = get_parent_name(nameB)
        if(actionA != 'DEL' or actionB != 'ADD'):
            return False
        if(int(timeB) - int(timeA) > 1):
            return False #assuming that the timeinterval should be really short. if not, give the benefit of doubt as different transactions
        isrename = _is_rename((filenameA,parentnameA,contenthashA),(filenameB,parentnameB,contenthashB))
        isupdate = _is_update((filenameA,parentnameA,contenthashA),(filenameB,parentnameB,contenthashB))
        ismove = _is_move((filenameA,parentnameA,contenthashA),(filenameB,parentnameB,contenthashB))
        isredundant = _is_redundant((filenameA,parentnameA,contenthashA),(filenameB,parentnameB,contenthashB))

        if(not(isrename or isupdate or ismove or isredundant)):
            return False
    return True

def _is_move(eventA,eventB):
    filenameA,parentnameA,contenthashA = eventA
    filenameB,parentnameB,contenthashB = eventB
    return True if (parentnameA != parentnameB and filenameA == filenameB and contenthashA == contenthashB) else False

def _is_update(eventA,eventB):
    filenameA,parentnameA,contenthashA = eventA
    filenameB,parentnameB,contenthashB = eventB
    return True if (parentnameA == parentnameB and filenameA == filenameB and contenthashA != contenthashB) else False

def _is_rename(eventA,eventB):
    filenameA,parentnameA,contenthashA = eventA
    filenameB,parentnameB,contenthashB = eventB
    return True if (parentnameA == parentnameB and filenameA != filenameB and contenthashA == contenthashB) else False
def _is_redundant(eventA,eventB):
    filenameA,parentnameA,contenthashA = eventA
    filenameB,parentnameB,contenthashB = eventB
    return True if (parentnameA == parentnameB and filenameA == filenameB and contenthashA == contenthashB) else False

def get_parent_name(name):
    return name[:name.rindex('/')+1]

def get_file_name(name):
    return name[name.rindex(r'/')+1:]
#say that the directory was deleted
def _can_combine_deletes(trans):
    pass
#say that a directory was created and files are added to the directory
def _can_combine_adds(trans):
    pass
''' 
    display options
    ---------------
    s - standard output (same as option 't')
    t - standard output(displays summary of events in the order of action)(default)
    v - verbose output (displays detailed output)
    f - standard output(displays summary of events with a file-centric view)
'''
def solution(events,display_option='s'):
    transactions = []
    count = 0
    prevtime = None
    #STEP1: standardize events by clustering
    for event in events:
        action,time,name,contenthash = event.split(' ')
        if time == prevtime:#Cluster events based on time
            transactions[count-1].append((event))
        else:
            transactions.append([(event)])
            count += 1
        prevtime = time
    #STEP2: futher combine transactions by actions such as add,delete,rename,move,update
    i=0
    file_table = {}
    while(i < count-1):
        if _can_combine_transactions(transactions[i],transactions[i+1]):
            event1 = transactions[i][0]
            event2 = transactions[i+1][0]
            action1,time1,name1,contenthash1 = event1.split(' ')
            action2,time2,name2,contenthash2 = event2.split(' ')
            filename1 = get_file_name(name1)
            parentname1 = get_parent_name(name1)
            filename2 = get_file_name(name2)
            parentname2 = get_parent_name(name2)

            file_to_change = None
            if filename1 in file_table:
                file_to_change = file_table[filename1]
            else:
                file_to_change = File(filename1,parentname1,time,contenthash1,ignore_create_action=True)
                file_table[filename1] = file_to_change

            if _is_rename((filename1,parentname1,contenthash1),(filename2,parentname2,contenthash2)):
                print filename1 + ' renamed to ' + filename2
                file_to_change.rename(time,filename2)
            elif _is_move((filename1,parentname1,contenthash1),(filename2,parentname2,contenthash2)):
                print filename1 + ' moved from ' + parentname1 + ' to ' + parentname2
                file_to_change.move(time,parentname2)
            elif _is_update((filename1,parentname1,contenthash1),(filename2,parentname2,contenthash2)):
                print filename1 + ' contents are updated'
                file_to_change(time,contenthash2)
            elif _is_redundant((filename1,parentname1,contenthash1),(filename2,parentname2,contenthash2)):
                pass
            else:
                print 'Program terminates because programmer did not think about this situation'
                raise
            i+=1
        else:
            for event in transactions[i]:
                action,time,name,contenthash = event.split(' ')
                parentname = get_parent_name(name)
                filename = get_file_name(name)
                if action == 'ADD':
                    print filename + ' created '
                    new_file = File(filename,parentname,time,contenthash)
                    file_table[filename] = new_file
                elif action == 'DEL':
                    if filename in file_table:
                        existing_file = file_table[filename]
                        existing_file.delete(time)
                    else:#The file could already exist in the dropbox folder. in that case we just create the filename w\o recording the create action.
                        new_file = File(filename,parentname,time,contenthash,ignore_create_action=True)
                        file_table[filename] = new_file
                        new_file.delete(time)
                    print filename + ' deleted '
        i+=1
    #for _,o_file in file_table.iteritems():
    #    print o_file.events


def test_sample_cases():
    '''
        test case for Rename:
        -----------------------
        6
        ADD 1282352346 /test -
        ADD 1282353016 /test/1.txt f2fa762f
        DEL 1282354012 /test -
        DEL 1282354012 /test/1.txt f2fa762f
        ADD 1282354013 /test2 -
        ADD 1282354013 /test2/1.txt f2fa762f

        test case for Move action:
        ---------------------------
        7
        ADD 0000000001 /parent1 -
        ADD 0000000002 /parent1/child.txt f2fa762f
        ADD 0000000003 /parent2 -
        DEL 0000000004 /parent1 -
        DEL 0000000004 /parent1/child.txt f2fa762f
        ADD 0000000005 /parent2/parent1 -
        ADD 0000000005 /parent2/parent1/child.txt f2fa762f
        
    '''
    events = []
    with open(r'test_reporting_file_events.txt','r') as fhandle:
        n = int(fhandle.readline().strip())
        i = 0
        while(i < n):
            events.append(fhandle.readline().strip())
            i+=1
    solution(events)

def test_file():
    dp_file = File(hash("test.txt"),"1282352346","-")
    assert dp_file.created_time == "1282352346"
    assert dp_file.modified_time == "1282352346"
    assert dp_file.name == "test.txt"
    assert dp_file.content_hash == "-"
    assert dp_file.status == "created"
    dp_file.remove("1282354012")
    assert dp_file.modified_time == "1282354012"
    assert dp_file.status == "deleted"
    dp_file.modify("1282354013")
    assert dp_file.modified_time == "1282354013"
    assert dp_file.status == "modified"
    print 'test_file tests pass'

if __name__ == '__main__':
    #test_file()
    test_sample_cases()
