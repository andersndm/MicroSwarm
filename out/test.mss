service Milking {
    root {
        LivestockId: EK;
        MilkContainerId: EK;
        Milk: float;
        Fat: float;
        Lactose: float;
        Protein: float;
        CellCount: int;
        Viable: bool;
    }
}

service LivestockOrder {
    root {
        OrderNo: string;
        StakeholderId: EK;
        LiveStock: List<EK>;
        Price: float;
        Payed: bool;
    }
}