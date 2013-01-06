if __name__ == '__main__':
    stages = 6
    entry_load = [2,4]
    exit_load = [3,2]
    transfer_cost = {(1,1):2,(2,1):2,(1,2):3,(2,2):1,(1,3):1,(2,3):2,(1,4):3,(2,4):2,(1,5):4,(2,5):1}
    operation_cost = {(1,1):7,(2,1):8,(1,2):9,(2,2):5,(1,3):3,(2,3):6,(1,4):4,(2,4):4,(1,5):8,(2,5):5,(1,6):4,(2,6):7}
    total_cost = {}

    total_cost[1,1] = entry_load[0] + operation_cost[1,1]
    total_cost[2,1] = entry_load[1] + operation_cost[2,1]

    for i in range(2,stages + 1,1):
        total_cost[1,i] = min(total_cost[1,i-1] + operation_cost[1,i],transfer_cost[2,i-1] + total_cost[2,i-1] + operation_cost[1,i])
        total_cost[2,i] = min(total_cost[2,i-1] + operation_cost[2,i],transfer_cost[1,i-1] + total_cost[1,i-1] + operation_cost[2,i])
    best_cost = min(exit_load[0] + total_cost[1,6],exit_load[1] + total_cost[2,6])
    print best_cost
