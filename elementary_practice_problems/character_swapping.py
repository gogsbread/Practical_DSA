def CharacterSwap(inputString):
    middle = len(inputString)/2
    length = len(inputString)
    inputList = list(inputString)
    for i in range(middle):
        temp = inputList[i]
        inputList[i] = inputList[length - 1 -i]
        inputList[length -1-i] = temp
    return ''.join(inputList) 

if __name__ == '__main__':
    print CharacterSwap('mdam')
    print CharacterSwap("Madam, I'm Adam")
