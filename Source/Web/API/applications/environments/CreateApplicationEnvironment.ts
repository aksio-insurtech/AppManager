/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { Command, CommandValidator, CommandPropertyValidators, useCommand, SetCommandValues, ClearCommandValues } from '@aksio/cratis-applications-frontend/commands';
import { Validator } from '@aksio/cratis-applications-frontend/validation';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/applications/{{applicationId}}/environments');

export interface ICreateApplicationEnvironment {
    applicationId?: string;
    environmentId?: string;
    name?: string;
    displayName?: string;
    shortName?: string;
    azureSubscriptionId?: string;
    cloudLocation?: string;
}

export class CreateApplicationEnvironmentValidator extends CommandValidator {
    readonly properties: CommandPropertyValidators = {
        applicationId: new Validator(),
        environmentId: new Validator(),
        name: new Validator(),
        displayName: new Validator(),
        shortName: new Validator(),
        azureSubscriptionId: new Validator(),
        cloudLocation: new Validator(),
    };
}

export class CreateApplicationEnvironment extends Command<ICreateApplicationEnvironment> implements ICreateApplicationEnvironment {
    readonly route: string = '/api/applications/{{applicationId}}/environments';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;
    readonly validation: CommandValidator = new CreateApplicationEnvironmentValidator();

    private _applicationId!: string;
    private _environmentId!: string;
    private _name!: string;
    private _displayName!: string;
    private _shortName!: string;
    private _azureSubscriptionId!: string;
    private _cloudLocation!: string;

    constructor() {
        super(Object, false);
    }

    get requestArguments(): string[] {
        return [
            'applicationId',
        ];
    }

    get properties(): string[] {
        return [
            'applicationId',
            'environmentId',
            'name',
            'displayName',
            'shortName',
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
    get environmentId(): string {
        return this._environmentId;
    }

    set environmentId(value: string) {
        this._environmentId = value;
        this.propertyChanged('environmentId');
    }
    get name(): string {
        return this._name;
    }

    set name(value: string) {
        this._name = value;
        this.propertyChanged('name');
    }
    get displayName(): string {
        return this._displayName;
    }

    set displayName(value: string) {
        this._displayName = value;
        this.propertyChanged('displayName');
    }
    get shortName(): string {
        return this._shortName;
    }

    set shortName(value: string) {
        this._shortName = value;
        this.propertyChanged('shortName');
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

    static use(initialValues?: ICreateApplicationEnvironment): [CreateApplicationEnvironment, SetCommandValues<ICreateApplicationEnvironment>, ClearCommandValues] {
        return useCommand<CreateApplicationEnvironment, ICreateApplicationEnvironment>(CreateApplicationEnvironment, initialValues);
    }
}
