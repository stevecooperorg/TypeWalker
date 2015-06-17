/* NamespaceOfTestClasses.BasicClass */
declare module KOGenerated.NamespaceOfTestClasses {
    export interface BasicClass {
        GetterPrivateSetterString(): number;
        GetterPrivateSetterString(value: number): void;
        NullableGetterSetterBool(): boolean;
        NullableGetterSetterBool(value: boolean): void;
        NavigationArray(): ];
        NavigationArray(value: any[]): void;
        NavigationProperty(): KOGenerated.NamespaceOfTestClasses.ReferencedClass;
        NavigationProperty(value: KOGenerated.NamespaceOfTestClasses.ReferencedClass): void;
        NavigationProperty2(): KOGenerated.NamespaceOfTestClasses.ReferencedClass;
        NavigationProperty2(value: KOGenerated.NamespaceOfTestClasses.ReferencedClass): void;
        StringField(): string;
        StringField(value: string): void;
    }
}

/* NamespaceOfTestClasses.ReferencedClass */
declare module KOGenerated.NamespaceOfTestClasses {
    export interface ReferencedClass {
        SelfReference(): KOGenerated.NamespaceOfTestClasses.ReferencedClass;
        SelfReference(value: KOGenerated.NamespaceOfTestClasses.ReferencedClass): void;
        BackReference(): KOGenerated.NamespaceOfTestClasses.BasicClass;
        BackReference(value: KOGenerated.NamespaceOfTestClasses.BasicClass): void;
    }
}