/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { Command, CommandValidator, CommandPropertyValidators, useCommand, SetCommandValues, ClearCommandValues } from '@aksio/cratis-applications-frontend/commands';
import { Validator } from '@aksio/cratis-applications-frontend/validation';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/applications/{{applicationId}}/environments/{{environmentId}}/tenants');

export interface IAddTenant {
    applicationId?: string;
    environmentId?: string;
    tenantId?: string;
    name?: string;
    shortName?: string;
}

export class AddTenantValidator extends CommandValidator {
    readonly properties: CommandPropertyValidators = {
        applicationId: new Validator(),
        environmentId: new Validator(),
        tenantId: new Validator(),
        name: new Validator(),
        shortName: new Validator(),
    };
}

export class AddTenant extends Command<IAddTenant> implements IAddTenant {
    readonly route: string = '/api/applications/{{applicationId}}/environments/{{environmentId}}/tenants';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;
    readonly validation: CommandValidator = new AddTenantValidator();

    private _applicationId!: string;
    private _environmentId!: string;
    private _tenantId!: string;
    private _name!: string;
    private _shortName!: string;

    get requestArguments(): string[] {
        return [
            'applicationId',
            'environmentId',
        ];
    }

    get properties(): string[] {
        return [
            'applicationId',
            'environmentId',
            'tenantId',
            'name',
            'shortName',
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
    get name(): string {
        return this._name;
    }

    set name(value: string) {
        this._name = value;
        this.propertyChanged('name');
    }
    get shortName(): string {
        return this._shortName;
    }

    set shortName(value: string) {
        this._shortName = value;
        this.propertyChanged('shortName');
    }

    static use(initialValues?: IAddTenant): [AddTenant, SetCommandValues<IAddTenant>, ClearCommandValues] {
        return useCommand<AddTenant, IAddTenant>(AddTenant, initialValues);
    }
}
