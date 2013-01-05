items = [(3,7),(4,9),(2,5),(6,12),(7,14),(3,6),(5,12)]
def knap(bag_size,item):
    if item < 0 or bag_size <= 0:
        return 0
    size,value = items[item]
    if size > bag_size:
        return knap(bag_size,item - 1)
    return max(knap(bag_size-size,item-1) + value, knap(bag_size,item-1))


if __name__ == '__main__':
    print knap(15,6)
