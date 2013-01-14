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
    def __init__(self,name,parentname,time,contenthash):
        self.created_time = time
        self.modified_time = time
        self.name = name
        self.parentname = parentname
        self.content_hash = contenthash
        self.status = "c"
        self.events = [("c",time)]
    def delete(self,time):
        self.status = "d"
        self.events.append(("d",self.time))
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

'''class DropBox:
    def __init__(self):
        #making a strong assumption that .DP will not be used by any other file
        self._ft = {}
        self._dels = []#tracks deletes at a transaction level
        self._adds = []#tracks adds at a transaction level

    def add(self,name,time,contenthash):
        if len(_adds) > 0 and _adds[0][0] != time:
            process_lasttransaction()#getting into a new trans; so process last tran
        self._adds.append((time,name,contenthash))
        if len(_dels) > 0:#track only when there are deletes going on so this add could be a part of the different trans
            self._adds.append((time,name,contenthash))
        else:#just process it
            object_name = get_object_name(name)
            parent_name = get_parent_name(name)
            newFile = File(object_name,parent_name,time,contenthash)
            self._ft.add(object_name,newFile)

    def del(self,name,time,contenthash):
        if len(_dels) > 0 and _dels[0][0] != time:
            process_lasttransaction()#getting into a new trans; so process last tran
        self._dels.append((time,name,contenthash))


    def _process_lasttransaction(self):
        if(len(_dels) == 0):
            return
        if(len(_dels) == len(_adds) and _dels[0][0] == _adds[0]0]): #belong to a single transaction , either move,rename or update
            affected_del = _dels[0]#you only care about the first affected object since this is part of the same transaction
            affected_add = _adds[0]
            if _is_move(affect_del,affected_add):
                time,name,contenthash = affected_add
                object_name = get_object_name(affected_del[1])
                parentname = get_parent_name(name)
                obj = self._ft[object_name]
                obj.move(time,parentname)
            elif _is_rename(affected_del,affected_add):
                time,name,contenthash = affected_add
                object_name = get_object_name(affected_del[1])
                newname = get_object_name(name)
                obj = self._ft[object_name]
                obj.rename(time,newname)
            elif _is_update(affected_del,affected_add):
                time,name,contenthash = affected_add
                object_name = get_object_name(affected_del[1])
                obj = self._ft[object_name]
                obj.update(time,contenthash)
            else:
                print 'Unfamilar condition that the program is not trained to handle'
                raise
        #individual delete actions; do not belong to a transaction.
        for time,name,contenthash in _dels:
            object_name = get_object_name(name)
            obj = self._ft[object_name]
            obj.delete(time)
        #individual add actions; do not belong to a transaction.
        for time,name,contenthash in _adds:
            object_name = get_object_name(name)
            parent_name = get_parent_name(name)
            newFile = File(object_name,parent_name,time,contenthash)
            self._ft.add(object_name,newFile)
        self._dels = list()
        self._adds = list()

    def _is_move(del_action,add_action):
        #move will result in a different parent but will have the same name & contenthash
        oldname,_,oldcontenthash = del_action
        newname,_,newcontenthash = add_action
        old_parent_name = get_parent_name(oldname)
        new_parent_name = get_parent_name(newname)
        old_file_name = get_object_name(oldname)
        new_file_name = get_object_name(newname)
        return True if (old_parent_name != new_parent_name and old_file_name == new_file_name and oldcontenthash == newcontenthash) else False

    def _is_rename(del_action,add_action):
        #rename will result in a different name but will retain the parent & contenthash
        oldname,_,oldcontenthash = del_action
        newname,_,newcontenthash = add_action
        old_parent_name = get_parent_name(oldname)
        new_parent_name = get_parent_name(newname)
        old_file_name = get_object_name(oldname)
        new_file_name = get_object_name(newname)
        return True if (old_parent_name == new_parent_name and old_file_name != new_file_name and oldcontenthash == newcontenthash) else False 

    def _is_update(del_action,add_action):
        #rename will result in a contenthash but will retain the parent & name
        oldname,_,oldcontenthash = del_action
        newname,_,newcontenthash = add_action
        old_parent_name = get_parent_name(oldname)
        new_parent_name = get_parent_name(newname)
        old_file_name = get_object_name(oldname)
        new_file_name = get_object_name(newname)
        return True if (old_parent_name == new_parent_name and old_file_name == new_file_name and oldcontenthash != newcontenthash) else False'''




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
        if(not(isrename or isupdate or ismove)):
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

def get_parent_name(name):
    return name[:name.rindex('/')]

def get_file_name(name):
    return name[name.rindex(r'/'):]


def solution(events):
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
    while(i < count-1):
        if _can_combine_transactions(transactions[i],transactions[i+1]):
            trans = transactions.pop(i+1)
            transactions[i].extend(trans)
            i+=1
        i+=1
    print transactions
    #process thru all the list and record only the first action
    for tran in transactions:

def test_sample_cases():
    events = ['ADD 1282352346 /test -','ADD 1282353016 /test/1.txt f2fa762f','DEL 1282354012 /test -','DEL 1282354012 /test/1.txt f2fa762f','ADD 1282354013 /test2 -','ADD 1282354013 /test2/1.txt f2fa762f']
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
