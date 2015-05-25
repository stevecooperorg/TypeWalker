module TypeWalker.Tests {
    export interface BasicClass {
        GetterSetterString: string;
        GetterPrivateSetterString: number;
        NullableGetterSetterBool: boolean;
        NavigationProperty: TypeWalker.Tests.ReferencedClass;
        NavigationProperty2: TypeWalker.Tests.ReferencedClass;
        StringField: String;
    }
}

module TypeWalker.Tests {
    export interface ReferencedClass {
        SelfReference: ReferencedClass;
        BackReference: TypeWalker.Tests.BasicClass;
    }
}