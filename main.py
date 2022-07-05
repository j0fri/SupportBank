import csv

def print_balances(accounts):
    for account in accounts:
        print(f"Name: {account}, Balance: {accounts[account][0]:.2f}")

def print_account(name, account_data):
    print(f"Transactions of: {name}")
    for data in account_data:
        print(data)

with open('C:\\Users\\jorfri\\Work\\Training\\SupportBank\\Transactions2014.csv', newline='\n') as csvfile:
    transactionReader = csv.reader(csvfile, delimiter=',')

    accounts = dict()
    next(transactionReader)

    for transaction in transactionReader:
        date = transaction[0]
        sender = transaction[1]
        receiver = transaction[2]
        narrative = transaction[3]
        amount = float(transaction[4])
        if not sender in accounts:
            accounts[sender] = [0,[]]
        if not receiver in accounts:
            accounts[receiver] = [0,[]]
        accounts[sender][0] -= amount
        accounts[sender][1].append(f"Date: {date}, Narrative: {narrative}, Sender: {sender}, Receiver: {receiver}, Amount: {amount:.2f}")
        accounts[receiver][0] += amount
        accounts[receiver][1].append(f"Date: {date}, Narrative: {narrative}, Sender: {sender}, Receiver: {receiver}, Amount: {amount:.2f}")

while True:
    command = input("Input command: ")
    if command == "List All":
        print_balances(accounts)
        continue
    command_strings = command.split(' ')
    if command_strings[0] == "List":
        name = ' '.join(command_strings[1:])
        if not name in accounts:
            print("That account doesn't exist!")
        else:
            print_account(name, accounts[name][1])
