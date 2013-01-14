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
class File:
    def __init__(self,name,time,contenthash):
        self.created_time = time
        self.modified_time = time
        self.name = name
        self.content_hash = contenthash
        self.status = "c"
        self.events = ["c"]
    def __eq__(self,file1,file2):
        return True if (file1.content_hash == file2.content_hash) else False
    def delete(self,time):
        self.status = "d"
        self.modified_time = time
        self.events.append("d")
    def update(self,time):
        self.status = "u"
        self.modified_time = time
        self.events.append("u")
    def rename(self,time):
        self.status = "r"
        self.modified_time = time
        self.events.append("r")
    def move(self,time):
        self.status = "m"
        self.modified_time = time
        self.events.append("m")
    def __hash__(self):
        return int(str(self.created_time) + str(name) 

class DropBox:
    def __init__(self):
        #making a strong assumption that .DP will not be used by any other file
        self._ft = {}

    def add(self,name,time,contenthash):

    def del(self,name,time,contenthash):
        pass

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
    test_file()

