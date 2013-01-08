def mulTables(n):
    for i in range(1,n+1):
        for j in range(1,n + 1):
            print i * j,
        print

if __name__ == '__main__':
    mulTables(12)
