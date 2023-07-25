/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { Command, CommandValidator, CommandPropertyValidators, useCommand, SetCommandValues, ClearCommandValues } from '@aksio/applications/commands';
import { Validator } from '@aksio/applications/validation';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/applications/{{applicationId}}/environments/{{environmentId}}/microservices/{{microserviceId}}/deployables/{{deployableId}}/environment-variables');

export interface ISetEnvironmentVariableForDeployable {
    applicationId?: string;
    environmentId?: string;
    microserviceId?: string;
    deployableId?: string;
    key?: string;
    value?: string;
}

export class SetEnvironmentVariableForDeployableValidator extends CommandValidator {
    readonly properties: CommandPropertyValidators = {
        applicationId: new Validator(),
        environmentId: new Validator(),
        microserviceId: new Validator(),
        deployableId: new Validator(),
        key: new Validator(),
        value: new Validator(),
    };
}

export class SetEnvironmentVariableForDeployable extends Command<ISetEnvironmentVariableForDeployable> implements ISetEnvironmentVariableForDeployable {
    readonly route: string = '/api/applications/{{applicationId}}/environments/{{environmentId}}/microservices/{{microserviceId}}/deployables/{{deployableId}}/environment-variables';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;
    readonly validation: CommandValidator = new SetEnvironmentVariableForDeployableValidator();

    private _applicationId!: string;
    private _environmentId!: string;
    private _microserviceId!: string;
    private _deployableId!: string;
    private _key!: string;
    private _value!: string;

    constructor() {
        super(Object, false);
    }

    get requestArguments(): string[] {
        return [
            'applicationId',
            'environmentId',
            'microserviceId',
            'deployableId',
        ];
    }

    get properties(): string[] {
        return [
            'applicationId',
            'environmentId',
            'microserviceId',
            'deployableId',
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
    get microserviceId(): string {
        return this._microserviceId;
    }

    set microserviceId(value: string) {
        this._microserviceId = value;
        this.propertyChanged('microserviceId');
    }
    get deployableId(): string {
        return this._deployableId;
    }

    set deployableId(value: string) {
        this._deployableId = value;
        this.propertyChanged('deployableId');
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

    static use(initialValues?: ISetEnvironmentVariableForDeployable): [SetEnvironmentVariableForDeployable, SetCommandValues<ISetEnvironmentVariableForDeployable>, ClearCommandValues] {
        return useCommand<SetEnvironmentVariableForDeployable, ISetEnvironmentVariableForDeployable>(SetEnvironmentVariableForDeployable, initialValues);
    }
}
