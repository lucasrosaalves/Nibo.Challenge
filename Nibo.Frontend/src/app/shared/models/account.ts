import { Transaction } from './transaction';

export interface AccountData {
    bank: string;
    accountNumber: string;
    balance: string;
    transactions: Transaction[];
}