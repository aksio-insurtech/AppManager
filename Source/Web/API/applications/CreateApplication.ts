/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { Command, CommandValidator, CommandPropertyValidators, useCommand, SetCommandValues, ClearCommandValues } from '@aksio/applications/commands';
import { Validator } from '@aksio/applications/validation';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/applications');

export interface ICreateApplication {
    applicationId?: string;
    name?: string;
    sharedAzureSubscriptionId?: string;
}

export class CreateApplicationValidator extends CommandValidator {
    readonly properties: CommandPropertyValidators = {
        applicationId: new Validator(),
        name: new Validator(),
        sharedAzureSubscriptionId: new Validator(),
    };
}

export class CreateApplication extends Command<ICreateApplication> implements ICreateApplication {
    readonly route: string = '/api/applications';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;
    readonly validation: CommandValidator = new CreateApplicationValidator();

    private _applicationId!: string;
    private _name!: string;
    private _sharedAzureSubscriptionId!: string;

    constructor() {
        super(Object, false);
    }

    get requestArguments(): string[] {
        return [
        ];
    }

    get properties(): string[] {
        return [
            'applicationId',
            'name',
            'sharedAzureSubscriptionId',
        ];
    }

    get applicationId(): string {
        return this._applicationId;
    }

    set applicationId(value: string) {
        this._applicationId = value;
        this.propertyChanged('applicationId');
    }
    get name(): string {
        return this._name;
    }

    set name(value: string) {
        this._name = value;
        this.propertyChanged('name');
    }
    get sharedAzureSubscriptionId(): string {
        return this._sharedAzureSubscriptionId;
    }

    set sharedAzureSubscriptionId(value: string) {
        this._sharedAzureSubscriptionId = value;
        this.propertyChanged('sharedAzureSubscriptionId');
    }

    static use(initialValues?: ICreateApplication): [CreateApplication, SetCommandValues<ICreateApplication>, ClearCommandValues] {
        return useCommand<CreateApplication, ICreateApplication>(CreateApplication, initialValues);
    }
}
