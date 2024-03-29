/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { Command, CommandValidator, CommandPropertyValidators, useCommand, SetCommandValues, ClearCommandValues } from '@aksio/applications/commands';
import { Validator } from '@aksio/applications/validation';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/applications/{{applicationId}}/environments/{{environmentId}}/secrets');

export interface ISetSecretForApplicationEnvironment {
    applicationId?: string;
    environmentId?: string;
    key?: string;
    value?: string;
}

export class SetSecretForApplicationEnvironmentValidator extends CommandValidator {
    readonly properties: CommandPropertyValidators = {
        applicationId: new Validator(),
        environmentId: new Validator(),
        key: new Validator(),
        value: new Validator(),
    };
}

export class SetSecretForApplicationEnvironment extends Command<ISetSecretForApplicationEnvironment> implements ISetSecretForApplicationEnvironment {
    readonly route: string = '/api/applications/{{applicationId}}/environments/{{environmentId}}/secrets';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;
    readonly validation: CommandValidator = new SetSecretForApplicationEnvironmentValidator();

    private _applicationId!: string;
    private _environmentId!: string;
    private _key!: string;
    private _value!: string;

    constructor() {
        super(Object, false);
    }

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

    static use(initialValues?: ISetSecretForApplicationEnvironment): [SetSecretForApplicationEnvironment, SetCommandValues<ISetSecretForApplicationEnvironment>, ClearCommandValues] {
        return useCommand<SetSecretForApplicationEnvironment, ISetSecretForApplicationEnvironment>(SetSecretForApplicationEnvironment, initialValues);
    }
}
