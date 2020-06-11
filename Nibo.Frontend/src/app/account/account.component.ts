import { Component, OnInit } from '@angular/core';
import { AccountData } from '../shared/models/account';
import { AccountService } from '../core/services/account.service';
import { Transaction } from '../shared/models/transaction';

@Component({
  selector: 'app-account',
  templateUrl: './account.component.html',
  styleUrls: ['./account.component.css']
})
export class AccountComponent implements OnInit {
  accounts: AccountData[] = [];
  selectedAccount: AccountData;
  paginatedTransactions: Transaction[] = [];
  hasErro: boolean;

  page:number;
  pageSize:number = 10;
  totalItems:number;
  totalPages: number;
  
  constructor(private accountService: AccountService) { }

  ngOnInit() {
    this.getAll();
    this.hasErro = false;
  }

  getAll(){
    this.accountService.getAll().subscribe(data =>{
      this.accounts = Object.assign([], data);
      console.log(this.accounts);
      this.selectedAccount = null;
    }, erro =>{
      this.accounts = [];
      this.selectedAccount = null;
      this.hasErro = true;
    });
  }

  closeAlert(){
    this.hasErro = false;
  }

  selectAccount(account: AccountData){
    this.selectedAccount = Object.assign({}, account);

    this.totalItems = this.selectedAccount.transactions.length;
    this.totalPages = Math.round(this.totalItems  / this.pageSize);

    this.paginate(this.selectedAccount.transactions, 1);
  }

  changePage(op: number) {
    const newPage = this.page + op;

    if (newPage === 0 || newPage === this.totalItems) {
      return;
    }

    const copy = Object.assign([], this.selectedAccount.transactions);

    this.paginate(copy, newPage);
  }

  paginate(data: Transaction[], page: number){

    this.page = page;

    this.paginatedTransactions =
    Object.assign([], this.selectedAccount.transactions)
    .slice((this.page - 1) * this.pageSize, (this.page - 1) * this.pageSize + this.pageSize);

  }

  backToList(){
    this.selectedAccount = null;
  }
}
