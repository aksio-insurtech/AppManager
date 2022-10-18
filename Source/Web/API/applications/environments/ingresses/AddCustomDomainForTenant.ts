/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { Command, CommandValidator, CommandPropertyValidators, useCommand, SetCommandValues } from '@aksio/cratis-applications-frontend/commands';
import { Validator } from '@aksio/cratis-applications-frontend/validation';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/applications/{{applicationId}}/environments/{{environmentId}}/ingresses/{{ingressId}}/custom-domain');

export interface IAddCustomDomainForTenant {
    applicationId?: string;
    environmentId?: string;
    ingressId?: string;
    tenantId?: string;
    domainName?: string;
}

export class AddCustomDomainForTenantValidator extends CommandValidator {
    readonly properties: CommandPropertyValidators = {
        applicationId: new Validator(),
        environmentId: new Validator(),
        ingressId: new Validator(),
        tenantId: new Validator(),
        domainName: new Validator(),
    };
}

export class AddCustomDomainForTenant extends Command<IAddCustomDomainForTenant> implements IAddCustomDomainForTenant {
    readonly route: string = '/api/applications/{{applicationId}}/environments/{{environmentId}}/ingresses/{{ingressId}}/custom-domain';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;
    readonly validation: CommandValidator = new AddCustomDomainForTenantValidator();

    private _applicationId!: string;
    private _environmentId!: string;
    private _ingressId!: string;
    private _tenantId!: string;
    private _domainName!: string;

    get requestArguments(): string[] {
        return [
            'applicationId',
            'environmentId',
            'ingressId',
        ];
    }

    get properties(): string[] {
        return [
            'applicationId',
            'environmentId',
            'ingressId',
            'tenantId',
            'domainName',
        ];
    }

    get applicationId(): string {
        return this._applicationId;
    }

    set applicationId(value: string) {
        this._applicationId = value;
        this.propertyChanged('applicationId');
    }
    get environmentId(): string {
        return this._environmentId;
    }

    set environmentId(value: string) {
        this._environmentId = value;
        this.propertyChanged('environmentId');
    }
    get ingressId(): string {
        return this._ingressId;
    }

    set ingressId(value: string) {
        this._ingressId = value;
        this.propertyChanged('ingressId');
    }
    get tenantId(): string {
        return this._tenantId;
    }

    set tenantId(value: string) {
        this._tenantId = value;
        this.propertyChanged('tenantId');
    }
    get domainName(): string {
        return this._domainName;
    }

    set domainName(value: string) {
        this._domainName = value;
        this.propertyChanged('domainName');
    }

    static use(initialValues?: IAddCustomDomainForTenant): [AddCustomDomainForTenant, SetCommandValues<IAddCustomDomainForTenant>] {
        return useCommand<AddCustomDomainForTenant, IAddCustomDomainForTenant>(AddCustomDomainForTenant, initialValues);
    }
}
