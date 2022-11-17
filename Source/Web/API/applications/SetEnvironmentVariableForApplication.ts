/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { Command, CommandValidator, CommandPropertyValidators, useCommand, SetCommandValues, ClearCommandValues } from '@aksio/cratis-applications-frontend/commands';
import { Validator } from '@aksio/cratis-applications-frontend/validation';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/applications/{{applicationId}}/environment-variable');

export interface ISetEnvironmentVariableForApplication {
    applicationId?: string;
    key?: string;
    value?: string;
}

export class SetEnvironmentVariableForApplicationValidator extends CommandValidator {
    readonly properties: CommandPropertyValidators = {
        applicationId: new Validator(),
        key: new Validator(),
        value: new Validator(),
    };
}

export class SetEnvironmentVariableForApplication extends Command<ISetEnvironmentVariableForApplication> implements ISetEnvironmentVariableForApplication {
    readonly route: string = '/api/applications/{{applicationId}}/environment-variable';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;
    readonly validation: CommandValidator = new SetEnvironmentVariableForApplicationValidator();

    private _applicationId!: string;
    private _key!: string;
    private _value!: string;

    get requestArguments(): string[] {
        return [
            'applicationId',
        ];
    }

    get properties(): string[] {
        return [
            'applicationId',
            'key',
            'value',
        ];
    }

    get applicationId(): string {
        return this._applicationId;
    }

    set applicationId(value: string) {
        this._applicationId = value;
        this.propertyChanged('applicationId');
    }
    get key(): string {
        return this._key;
    }

    set key(value: string) {
        this._key = value;
        this.propertyChanged('key');
    }
    get value(): string {
        return this._value;
    }

    set value(value: string) {
        this._value = value;
        this.propertyChanged('value');
    }

    static use(initialValues?: ISetEnvironmentVariableForApplication): [SetEnvironmentVariableForApplication, SetCommandValues<ISetEnvironmentVariableForApplication>, ClearCommandValues] {
        return useCommand<SetEnvironmentVariableForApplication, ISetEnvironmentVariableForApplication>(SetEnvironmentVariableForApplication, initialValues);
    }
}
