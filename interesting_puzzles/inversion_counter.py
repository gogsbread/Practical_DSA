'''
2 4 1 3 5
1 2 3 4 5
Inversions : (2,1), (4,1), (4,3)


approach:
------
    divide and conquer
        divide : split to subproblems which is one number
        conquer: return the sorted list
        merge: count the number of hops to be made for the string to be sorted. cumulatively count this hop
'''
import sys
def merge_couns(seq,head,middle,tail,total_count):
    left = seq[head:middle]
    right = seq[middle:tail]
    count = total_count
    left.append(sys.maxint)
    right.append(sys.maxint)
    i = j = 0
    k = len(left) + len(right)
    for k in range(head,tail,1):
        if (left[i] <= right[j]):
            seq[k] = left[i]
            i+=1
        else:
            count += 1
            seq[k] = right[j]
            j+=1
    return count

def count_inversions(seq,head,tail):
    if head == tail-1:
        return 0
    middle = (head+tail)/2
    left_count = count_inversions(seq,head,middle)
    right_count = count_inversions(seq,middle,tail)
    return merge_couns(seq,head,middle,tail,left_count+right_count)

if __name__ == '__main__':
    seq = [2,4,1,3,5]
    print count_inversions(seq,0,len(seq))
