/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { Command, CommandValidator, CommandPropertyValidators, useCommand, SetCommandValues } from '@aksio/cratis-applications-frontend/commands';
import { Validator } from '@aksio/cratis-applications-frontend/validation';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/applications/{{applicationId}}/{{environment}}/config');

export interface ISetConfig {
    applicationId?: string;
    environment?: string;
}

export class SetConfigValidator extends CommandValidator {
    readonly properties: CommandPropertyValidators = {
        applicationId: new Validator(),
        environment: new Validator(),
    };
}

export class SetConfig extends Command<ISetConfig> implements ISetConfig {
    readonly route: string = '/api/applications/{{applicationId}}/{{environment}}/config';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;
    readonly validation: CommandValidator = new SetConfigValidator();

    private _applicationId!: string;
    private _environment!: string;

    get requestArguments(): string[] {
        return [
            'applicationId',
            'environment',
        ];
    }

    get properties(): string[] {
        return [
            'applicationId',
            'environment',
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

    static use(initialValues?: ISetConfig): [SetConfig, SetCommandValues<ISetConfig>] {
        return useCommand<SetConfig, ISetConfig>(SetConfig, initialValues);
    }
}
