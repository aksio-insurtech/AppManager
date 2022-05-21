/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { Command, CommandValidator, CommandPropertyValidators, useCommand, SetCommandValues } from '@aksio/cratis-applications-frontend/commands';
import { Validator } from '@aksio/cratis-applications-frontend/validation';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/organization/settings/subscriptions');

export interface IAddAzureSubscription {
    id?: string;
    name?: string;
    tenantName?: string;
}

export class AddAzureSubscriptionValidator extends CommandValidator {
    readonly properties: CommandPropertyValidators = {
        id: new Validator(),
        name: new Validator(),
        tenantName: new Validator(),
    };
}

export class AddAzureSubscription extends Command<IAddAzureSubscription> implements IAddAzureSubscription {
    readonly route: string = '/api/organization/settings/subscriptions';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;
    readonly validation: CommandValidator = new AddAzureSubscriptionValidator();

    private _id!: string;
    private _name!: string;
    private _tenantName!: string;

    get requestArguments(): string[] {
        return [
        ];
    }

    get properties(): string[] {
        return [
            'id',
            'name',
            'tenantName',
        ];
    }

    get id(): string {
        return this._id;
    }

    set id(value: string) {
        this._id = value;
        this.propertyChanged('id');
    }
    get name(): string {
        return this._name;
    }

    set name(value: string) {
        this._name = value;
        this.propertyChanged('name');
    }
    get tenantName(): string {
        return this._tenantName;
    }

    set tenantName(value: string) {
        this._tenantName = value;
        this.propertyChanged('tenantName');
    }

    static use(initialValues?: IAddAzureSubscription): [AddAzureSubscription, SetCommandValues<IAddAzureSubscription>] {
        return useCommand<AddAzureSubscription, IAddAzureSubscription>(AddAzureSubscription, initialValues);
    }
}
