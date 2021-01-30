
export class ReportByNet {
  constructor(
    public storeName: string,

    public storesCount: number,

    public ordersCount: number,

    public soldOrders: number,

    public timeOutCanceled: number,

    public customerCanceledOrders: number,

    public noEndStatusOrd: number,

    public canceledOrd: number,

    public noReceiveStatusOrd: number) { }
}

