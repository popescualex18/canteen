export class CustomerModel {
    constructor(
       init:Partial<CustomerModel>
      ) { Object.assign(this, init);}
      public id!: string;
      public name!:string;
      public sutName!:string;
      public email!: string;
      public password!: string;
      public phoneNumber!:string; 
      public address!: string;
}