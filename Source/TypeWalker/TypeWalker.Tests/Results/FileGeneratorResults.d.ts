/* NamespaceOfTestClasses.BasicClass */
declare module NamespaceOfTestClasses {
    export interface BasicClass {
        GetterSetterString: string;
        GetterPrivateSetterString: number;
        NullableGetterSetterBool: boolean;
        NavigationArray: NamespaceOfTestClasses.ReferencedClass[];
        NavigationProperty: NamespaceOfTestClasses.ReferencedClass;
        NavigationProperty2: NamespaceOfTestClasses.ReferencedClass;
        StringField: string;
    }
}

/* NamespaceOfTestClasses.ReferencedClass */
declare module NamespaceOfTestClasses {
    export interface ReferencedClass {
        SelfReference: NamespaceOfTestClasses.ReferencedClass;
        BackReference: NamespaceOfTestClasses.BasicClass;
    }
}

/* AlternateNamespace.DistinctClass */
declare module AlternateNamespace {
    export interface DistinctClass {
        Foo: string;
        Backreference: NamespaceOfTestClasses.ReferencedClass;
    }
}