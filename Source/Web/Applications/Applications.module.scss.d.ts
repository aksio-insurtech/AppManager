declare namespace ApplicationsModuleScssNamespace {
    export interface IApplicationsModuleScss {
        actions: string;
        applicationsContainer: string;
        applicationsNavigation: string;
        itemActions: string;
    }
}

declare const ApplicationsModuleScssModule: ApplicationsModuleScssNamespace.IApplicationsModuleScss & {
    /** WARNING: Only available when `css-loader` is used without `style-loader` or `mini-css-extract-plugin` */
    locals: ApplicationsModuleScssNamespace.IApplicationsModuleScss;
};

export = ApplicationsModuleScssModule;
