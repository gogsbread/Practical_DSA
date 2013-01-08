import sys
import os

#items = [(3,7),(4,9),(2,5),(6,12),(7,14),(3,6),(5,12)]
items = []
count = 0
def memoizer(f):
    cache = {}
    def _f(*args):
        global count
        try:
            count += 1
            return cache[args]
        except KeyError:
            count -= 1
            value = cache[args] = f(*args)
            return value
        except TypeError:
            count -= 1
            return f(*args)
    return _f

@memoizer
def knap(bag_size,item):
    if item < 0 or bag_size <= 0:
        return 0
    size,value = items[item]
    if size > bag_size:
        return knap(bag_size,item - 1)
    return max(knap(bag_size-size,item-1) + value, knap(bag_size,item-1))

if __name__ == '__main__':
    #print knap(15,6)
    args = sys.argv
    if len(args)<2:
        sys.exit('Please input the test file name')
    f_path = args[1]
    if not os.path.exists(f_path):
        sys.exit(f_path + ' does not exist')
    sys.setrecursionlimit(2**31-1)
    with open(f_path,r'rU') as f_handle:
        line = f_handle.readline()
        capacity,n = line.split(' ')
        capacity = int(capacity)
        n = int(n)
        for item in range(n):
            line = f_handle.readline()
            value,weight = line.split(' ')
            items.append((int(weight),int(value)))
    #print items
    print knap(capacity,n-1)
    print count

