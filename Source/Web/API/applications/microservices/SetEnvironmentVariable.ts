/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { Command, CommandValidator, CommandPropertyValidators, useCommand, SetCommandValues } from '@aksio/cratis-applications-frontend/commands';
import { Validator } from '@aksio/cratis-applications-frontend/validation';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/applications/{{applicationId}}/{{environment}}/microservices/{{microserviceId}}/environment-variable');

export interface ISetEnvironmentVariable {
    applicationId?: string;
    environment?: string;
    microserviceId?: string;
}

export class SetEnvironmentVariableValidator extends CommandValidator {
    readonly properties: CommandPropertyValidators = {
        applicationId: new Validator(),
        environment: new Validator(),
        microserviceId: new Validator(),
    };
}

export class SetEnvironmentVariable extends Command<ISetEnvironmentVariable> implements ISetEnvironmentVariable {
    readonly route: string = '/api/applications/{{applicationId}}/{{environment}}/microservices/{{microserviceId}}/environment-variable';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;
    readonly validation: CommandValidator = new SetEnvironmentVariableValidator();

    private _applicationId!: string;
    private _environment!: string;
    private _microserviceId!: string;

    get requestArguments(): string[] {
        return [
            'applicationId',
            'environment',
            'microserviceId',
        ];
    }

    get properties(): string[] {
        return [
            'applicationId',
            'environment',
            'microserviceId',
        ];
    }

    get applicationId(): string {
        return this._applicationId;
    }

    set applicationId(value: string) {
        this._applicationId = value;
        this.propertyChanged('applicationId');
    }
    get environment(): string {
        return this._environment;
    }

    set environment(value: string) {
        this._environment = value;
        this.propertyChanged('environment');
    }
    get microserviceId(): string {
        return this._microserviceId;
    }

    set microserviceId(value: string) {
        this._microserviceId = value;
        this.propertyChanged('microserviceId');
    }

    static use(initialValues?: ISetEnvironmentVariable): [SetEnvironmentVariable, SetCommandValues<ISetEnvironmentVariable>] {
        return useCommand<SetEnvironmentVariable, ISetEnvironmentVariable>(SetEnvironmentVariable, initialValues);
    }
}
