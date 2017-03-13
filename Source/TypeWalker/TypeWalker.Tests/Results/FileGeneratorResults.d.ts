/* AlternateNamespace.DistinctClass */
declare module AlternateNamespace {
    export interface DistinctClass {
        Backreference: NamespaceOfTestClasses.ReferencedClass;
        Foo: string;
    }
}

/* NamespaceOfTestClasses.BasicClass */
declare module NamespaceOfTestClasses {
    export interface BasicClass {
        GetterPrivateSetterString: number;
        GetterSetterString: string;
        NavigationArray: NamespaceOfTestClasses.ReferencedClass[];
        NavigationProperty: NamespaceOfTestClasses.ReferencedClass;
        NavigationProperty2: NamespaceOfTestClasses.ReferencedClass;
        NullableGetterSetterBool: boolean;
        StringField: string;
    }
}

/* NamespaceOfTestClasses.ReferencedClass */
declare module NamespaceOfTestClasses {
    export interface ReferencedClass {
        BackReference: NamespaceOfTestClasses.BasicClass;
        SelfReference: NamespaceOfTestClasses.ReferencedClass;
    }
}

/* NamespaceOfTestClasses.Subclass */
declare module NamespaceOfTestClasses {
    export interface Subclass extends NamespaceOfTestClasses.BasicClass {
        SubclassesOwnProperty: string;
    }
}