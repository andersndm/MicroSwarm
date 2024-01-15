service Milking {
    root {
        LivestockId: int;
        MilkContainerId: int;
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
        StakeholderId: int;
        LiveStock: List<int>;
        Price: float;
        Payed: bool;
    }
}