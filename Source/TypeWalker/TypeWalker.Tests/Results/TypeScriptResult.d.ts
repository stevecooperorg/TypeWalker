declare module NamespaceOfTestClasses {
    export interface BasicClass {
        GetterSetterString: string;
        GetterPrivateSetterString: number;
        NullableGetterSetterBool: boolean;
        NavigationProperty: NamespaceOfTestClasses.ReferencedClass;
        NavigationProperty2: NamespaceOfTestClasses.ReferencedClass;
        StringField: string;
    }
}

declare module NamespaceOfTestClasses {
    export interface ReferencedClass {
        SelfReference: NamespaceOfTestClasses.ReferencedClass;
        BackReference: NamespaceOfTestClasses.BasicClass;
    }
}