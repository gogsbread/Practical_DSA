def Sum(filePath):
    total = 0
    with open(filePath,'rU') as f:
        line = f.readline()
        while(line != ''):
            value = int(line)
            total+=value
            line = f.readline()
    return total

if __name__ == '__main__':
    print Sum('./Tests/temp.txt')
