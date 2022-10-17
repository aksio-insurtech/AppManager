/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { Command, CommandValidator, CommandPropertyValidators, useCommand, SetCommandValues } from '@aksio/cratis-applications-frontend/commands';
import { Validator } from '@aksio/cratis-applications-frontend/validation';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/applications/{{applicationId}}/environments/{environment}/environments');

export interface IAddEnvironment {
    applicationId?: string;
    environmentId?: string;
    name?: string;
    displayName?: string;
    shortName?: string;
}

export class AddEnvironmentValidator extends CommandValidator {
    readonly properties: CommandPropertyValidators = {
        applicationId: new Validator(),
        environmentId: new Validator(),
        name: new Validator(),
        displayName: new Validator(),
        shortName: new Validator(),
    };
}

export class AddEnvironment extends Command<IAddEnvironment> implements IAddEnvironment {
    readonly route: string = '/api/applications/{{applicationId}}/environments/{environment}/environments';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;
    readonly validation: CommandValidator = new AddEnvironmentValidator();

    private _applicationId!: string;
    private _environmentId!: string;
    private _name!: string;
    private _displayName!: string;
    private _shortName!: string;

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

    static use(initialValues?: IAddEnvironment): [AddEnvironment, SetCommandValues<IAddEnvironment>] {
        return useCommand<AddEnvironment, IAddEnvironment>(AddEnvironment, initialValues);
    }
}
