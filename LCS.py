def LCS(S,m,T,n):
    if m == -1 or n == -1:
        return 0
    if S[m] == T[n]:
        return LCS(S,m-1,T,n-1) + 1
    else:
        return max(LCS(S,m-1,T,n),LCS(S,m,T,n-1))

def lcs_matrix(S,m,T,n):
    table = dict()
    for i in range(m + 1):
        for j in range(n+1):
            if i == 0 or j == 0:
                table[i,j] = 0
            elif S[i-1] == T[j-1]:
                table[i,j] = 1 + table[i-1,j-1]
            else:
                table[i,j] = max(table[i-1,j],table[i,j-1])
    for i in range(1,m+1):
        for j in range(1,n+1):
            print "{0}\t".format(table[i,j]),
        print 

if __name__ == '__main__':
    #print LCS('BACBAD',5,'ABAZDC',5)
    #lcs_matrix('BACBAD',6,'ABAZDC',6)
    print LCS([1,5,3,4,8,7,10],6,[1,3,4,5,7,8,10],6)

