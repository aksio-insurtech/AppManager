/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { Command, CommandValidator, CommandPropertyValidators, useCommand, SetCommandValues } from '@aksio/cratis-applications-frontend/commands';
import { Validator } from '@aksio/cratis-applications-frontend/validation';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/applications');

export interface ICreateApplication {
    applicationId?: string;
    name?: string;
    azureSubscriptionId?: string;
    cloudLocation?: string;
}

export class CreateApplicationValidator extends CommandValidator {
    readonly properties: CommandPropertyValidators = {
        applicationId: new Validator(),
        name: new Validator(),
        azureSubscriptionId: new Validator(),
        cloudLocation: new Validator(),
    };
}

export class CreateApplication extends Command<ICreateApplication> implements ICreateApplication {
    readonly route: string = '/api/applications';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;
    readonly validation: CommandValidator = new CreateApplicationValidator();

    private _applicationId!: string;
    private _name!: string;
    private _azureSubscriptionId!: string;
    private _cloudLocation!: string;

    get requestArguments(): string[] {
        return [
        ];
    }

    get properties(): string[] {
        return [
            'applicationId',
            'name',
            'azureSubscriptionId',
            'cloudLocation',
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
    get azureSubscriptionId(): string {
        return this._azureSubscriptionId;
    }

    set azureSubscriptionId(value: string) {
        this._azureSubscriptionId = value;
        this.propertyChanged('azureSubscriptionId');
    }
    get cloudLocation(): string {
        return this._cloudLocation;
    }

    set cloudLocation(value: string) {
        this._cloudLocation = value;
        this.propertyChanged('cloudLocation');
    }

    static use(initialValues?: ICreateApplication): [CreateApplication, SetCommandValues<ICreateApplication>] {
        return useCommand<CreateApplication, ICreateApplication>(CreateApplication, initialValues);
    }
}
