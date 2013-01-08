## {{{ http://code.activestate.com/recipes/576694/ (r9)
import collections

class OrderedSet(collections.MutableSet):

    def __init__(self, iterable=None):
        self.end = end = [] 
        end += [None, end, end]         # sentinel node for doubly linked list
        self.map = {}                   # key --> [key, prev, next]
        if iterable is not None:
            self |= iterable

    def __len__(self):
        return len(self.map)

    def __contains__(self, key):
        return key in self.map

    def add(self, key):
        if key not in self.map:
            end = self.end
            curr = end[1]
            curr[2] = end[1] = self.map[key] = [key, curr, end]

    def discard(self, key):
        if key in self.map:        
            key, prev, next = self.map.pop(key)
            prev[2] = next
            next[1] = prev

    def __iter__(self):
        end = self.end
        curr = end[2]
        while curr is not end:
            yield curr[0]
            curr = curr[2]

    def __reversed__(self):
        end = self.end
        curr = end[1]
        while curr is not end:
            yield curr[0]
            curr = curr[1]

    def pop(self, last=True):
        if not self:
            raise KeyError('set is empty')
        key = self.end[1][0] if last else self.end[2][0]
        self.discard(key)
        return key

    def __repr__(self):
        if not self:
            return '%s()' % (self.__class__.__name__,)
        return '%s(%r)' % (self.__class__.__name__, list(self))

    def __eq__(self, other):
        if isinstance(other, OrderedSet):
            return len(self) == len(other) and list(self) == list(other)
        return set(self) == set(other)

## end of http://code.activestate.com/recipes/576694/ }}}

count = 0
def memoizer(f):
    cache = {}
    def _fmem(*args):
        global count
        _,_,l,w,explored = args
        str_explored = ''
        for e in explored:
            str_explored += str(e)
        cache_args = (l,w,str_explored)
        try:
            count += 1
            return cache[cache_args]
        except KeyError:
            count -= 1
            cache[cache_args] = result = f(*args)
            return result
        except TypeError as e:
            count -= 1
            return f(*args)
    return _fmem

@memoizer
def buildTower(boxes,level,tl,tw,explored):
    totalheight = 0
    for i,box in enumerate(boxes):
        if i>level:
            l,w,h = box[0]
            inode = box[1]
            if inode not in explored and max(l,w) >= max(tl,tw) and min(l,w) >= min(tl,tw):
                new_explorations = OrderedSet(explored)
                new_explorations.add(inode)
                totalheight = max(totalheight,h + buildTower(boxes,i,l,w,new_explorations))
    return totalheight

def sort_by_base(box):
    l,w,_ = box[0]
    return l*w

def tower_of_boxes(boxes,debug=False):
    all_boxes = []
    for inode,box in enumerate(boxes):
        rotator = ((box[i%3],box[(i+1)%3],box[(i+2)%3]) for i in range(3))
        for r_box in rotator:
            all_boxes.append((r_box,inode))
    all_boxes.sort(key=sort_by_base)
    max_tower_height = buildTower(all_boxes,-1,0,0,OrderedSet())
    if debug:
        print str.format('Cache hits:{0}',count)
    return max_tower_height

def test():
    boxes=[(5,2,4),(1,4,2),(4,4,2)]#13
    #boxes=[(8,8,2),(4,4,4),(1,1,1)]#9
    #boxes=[(5,2,4),(1,4,2),(4,4,2),(20,3,4),(5,7,8),(5,7,3),(3,5,6)]#54
    #boxes=[(5,2,4),(1,4,2),(4,4,2),(20,3,4),(5,7,8),(5,7,3),(3,5,6),(4,6,7),(5,2,43),(67,21,12),(1,2,31),(45,32,35),(3,2,7)]#234
    print str.format('Max. tower height:{0}',tower_of_boxes(boxes,False))

if __name__ == '__main__':
    test()
