/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { Command, CommandValidator, CommandPropertyValidators, useCommand, SetCommandValues, ClearCommandValues } from '@aksio/cratis-applications-frontend/commands';
import { Validator } from '@aksio/cratis-applications-frontend/validation';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/applications/{{applicationId}}/environments/{{environmentId}}/tenants/{{tenantId}}/custom-domain');

export interface IAssociateDomainWithTenant {
    applicationId?: string;
    environmentId?: string;
    tenantId?: string;
    domain?: string;
    certificateId?: string;
}

export class AssociateDomainWithTenantValidator extends CommandValidator {
    readonly properties: CommandPropertyValidators = {
        applicationId: new Validator(),
        environmentId: new Validator(),
        tenantId: new Validator(),
        domain: new Validator(),
        certificateId: new Validator(),
    };
}

export class AssociateDomainWithTenant extends Command<IAssociateDomainWithTenant> implements IAssociateDomainWithTenant {
    readonly route: string = '/api/applications/{{applicationId}}/environments/{{environmentId}}/tenants/{{tenantId}}/custom-domain';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;
    readonly validation: CommandValidator = new AssociateDomainWithTenantValidator();

    private _applicationId!: string;
    private _environmentId!: string;
    private _tenantId!: string;
    private _domain!: string;
    private _certificateId!: string;

    constructor() {
        super(Object, false);
    }

    get requestArguments(): string[] {
        return [
            'applicationId',
            'environmentId',
            'tenantId',
        ];
    }

    get properties(): string[] {
        return [
            'applicationId',
            'environmentId',
            'tenantId',
            'domain',
            'certificateId',
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
    get tenantId(): string {
        return this._tenantId;
    }

    set tenantId(value: string) {
        this._tenantId = value;
        this.propertyChanged('tenantId');
    }
    get domain(): string {
        return this._domain;
    }

    set domain(value: string) {
        this._domain = value;
        this.propertyChanged('domain');
    }
    get certificateId(): string {
        return this._certificateId;
    }

    set certificateId(value: string) {
        this._certificateId = value;
        this.propertyChanged('certificateId');
    }

    static use(initialValues?: IAssociateDomainWithTenant): [AssociateDomainWithTenant, SetCommandValues<IAssociateDomainWithTenant>, ClearCommandValues] {
        return useCommand<AssociateDomainWithTenant, IAssociateDomainWithTenant>(AssociateDomainWithTenant, initialValues);
    }
}
