/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { Command, CommandValidator, CommandPropertyValidators, useCommand, SetCommandValues } from '@aksio/cratis-applications-frontend/commands';
import { Validator } from '@aksio/cratis-applications-frontend/validation';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/applications/{{applicationId}}/environments/{{environmentId}}/environment-variable');

export interface ISetEnvironmentVariableForApplicationEnvironment {
    applicationId?: string;
    environmentId?: string;
    key?: string;
    value?: string;
}

export class SetEnvironmentVariableForApplicationEnvironmentValidator extends CommandValidator {
    readonly properties: CommandPropertyValidators = {
        applicationId: new Validator(),
        environmentId: new Validator(),
        key: new Validator(),
        value: new Validator(),
    };
}

export class SetEnvironmentVariableForApplicationEnvironment extends Command<ISetEnvironmentVariableForApplicationEnvironment> implements ISetEnvironmentVariableForApplicationEnvironment {
    readonly route: string = '/api/applications/{{applicationId}}/environments/{{environmentId}}/environment-variable';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;
    readonly validation: CommandValidator = new SetEnvironmentVariableForApplicationEnvironmentValidator();

    private _applicationId!: string;
    private _environmentId!: string;
    private _key!: string;
    private _value!: string;

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
    get environmentId(): string {
        return this._environmentId;
    }

    set environmentId(value: string) {
        this._environmentId = value;
        this.propertyChanged('environmentId');
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

    static use(initialValues?: ISetEnvironmentVariableForApplicationEnvironment): [SetEnvironmentVariableForApplicationEnvironment, SetCommandValues<ISetEnvironmentVariableForApplicationEnvironment>] {
        return useCommand<SetEnvironmentVariableForApplicationEnvironment, ISetEnvironmentVariableForApplicationEnvironment>(SetEnvironmentVariableForApplicationEnvironment, initialValues);
    }
}
