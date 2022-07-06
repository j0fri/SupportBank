import csv
import logging
from datetime import datetime
import json
import xml.etree.ElementTree as ET


from datetime import datetime



logging.basicConfig(filename='.\\SupportBank.log', filemode='w', level=logging.DEBUG)

def print_balances(accounts):
    for account in accounts:
        print(f"Name: {account}, Balance: {accounts[account][0]:.2f}")

def print_account(name, account_data):
    print(f"Transactions of: {name}")
    for data in account_data:
        print(data)

def add_to_account(accounts, sender, receiver, amount, narrative, date):
    if not sender in accounts:
        accounts[sender] = [0, []]
    if not receiver in accounts:
        accounts[receiver] = [0, []]

    accounts[sender][0] -= amount
    accounts[sender][1].append(
        f"Date: {date}, Narrative: {narrative}, Sender: {sender}, Receiver: {receiver}, Amount: {amount:.2f}")
    accounts[receiver][0] += amount
    accounts[receiver][1].append(
        f"Date: {date}, Narrative: {narrative}, Sender: {sender}, Receiver: {receiver}, Amount: {amount:.2f}")

def convert_amount(amount_string, line = None):
    try:
        amount = float(amount_string)
    except ValueError:
        if type(line) == int:
            logging.warning(f"Line: {line + 2}, {amount_string} is not a valid amount.")
        else:
            logging.warning(f"{amount_string} is not a valid amount.")
        return 0.0, 1
    finally:
        return amount, 0

def read_csv(name):
    status = 0
    with open(name, "r", newline='\n') as csvfile:
        transaction_reader = csv.reader(csvfile, delimiter=',')
        accounts = dict()
        next(transaction_reader)

        for line, transaction in enumerate(list(transaction_reader)):
            date = transaction[0]
            try:
                datetime.strptime(date, "%d/%m/%Y")
            except ValueError:
                logging.warning(f"Line: {line+2}, {date} is not a valid date.")
                status = 1
                continue

            sender = transaction[1]
            receiver = transaction[2]
            narrative = transaction[3]

            amount, invalid = convert_amount(transaction[4], line)

            if invalid:
                status = 1
                continue

            add_to_account(accounts, sender, receiver, amount, narrative, date)

    return accounts, status

def read_json(filename):
    status = 0
    with open(filename, "r") as json_file:
        data = json.load(json_file)
        accounts = dict()

        for line, transaction in enumerate(data):
            date = transaction['Date']
            try:
                datetime.strptime(date, "%Y-%m-%dT%H:%M:%S")
            except ValueError:
                logging.warning(f"Line: {line + 2}, {date} is not a valid date.")
                status = 1
                continue

            sender = transaction['FromAccount']
            receiver = transaction['ToAccount']
            narrative = transaction['Narrative']

            amount, invalid = convert_amount(transaction[4], line)

            if invalid:
                status = 1
                continue

            add_to_account(accounts, sender, receiver, amount, narrative, date)

    return accounts, status

def read_xml(filename):
    data_tree = ET.parse(filename)
    root = data_tree.getroot()
    accounts = dict()
    status = 0
    for transaction_object in root:
        serial_date_string = transaction_object.attrib['Date']

        if serial_date_string.isdigit() and len(serial_date_string) == 5:
            serial_date = int(serial_date_string)
        else:
            logging.warning(f"{date} is not a valid date.")
            status = 1
            continue

        date_long = str(datetime.fromordinal(datetime(1900, 1, 1).toordinal() + serial_date - 2))
        date = date_long[0:10]
        narrative = transaction_object[0].text
        amount_string = transaction_object[1].text
        sender = transaction_object[2][0].text
        receiver = transaction_object[2][1].text

        amount, invalid = convert_amount(amount_string)

        if invalid:
            status = 1
            continue

        add_to_account(accounts, sender, receiver, amount, narrative, date)

    return accounts, status




def read_file(filename):
    extension = filename.split('.')[1]
    if extension == "csv":
        accounts, status = read_csv(filename)
    elif extension == "json":
        accounts, status = read_json(filename)
    elif extension == "xml":
        accounts, status = read_xml(filename)
    else:
        print("Invalid file extension.")

    return accounts, status

if __name__ == "__main__":
    filename = input("Enter the filename of the file you want to read from: ")
    accounts, status = read_file(filename)
    if status:
        print("Invalid data found. Please check log file for more details.")
    while True:
        command = input("Input command: ")

        if command == "List All":
            print_balances(accounts)
            continue
        command_strings = command.split(' ')

        if command_strings[0] + ' ' + command_strings[1] == "Import File":
            accounts, status = read_file(command_strings[2])

        if command_strings[0] == "List":
            name = ' '.join(command_strings[1:])
            if not name in accounts:
                print("That account doesn't exist!")
            else:
                print_account(name, accounts[name][1])
