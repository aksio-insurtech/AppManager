/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { Command, CommandValidator, CommandPropertyValidators, useCommand, SetCommandValues } from '@aksio/cratis-applications-frontend/commands';
import { Validator } from '@aksio/cratis-applications-frontend/validation';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/applications/{{applicationId}}/authentication');

export interface IConfigureAuthentication {
    applicationId?: string;
    clientId?: string;
    clientSecret?: string;
}

export class ConfigureAuthenticationValidator extends CommandValidator {
    readonly properties: CommandPropertyValidators = {
        applicationId: new Validator(),
        clientId: new Validator(),
        clientSecret: new Validator(),
    };
}

export class ConfigureAuthentication extends Command<IConfigureAuthentication> implements IConfigureAuthentication {
    readonly route: string = '/api/applications/{{applicationId}}/authentication';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;
    readonly validation: CommandValidator = new ConfigureAuthenticationValidator();

    private _applicationId!: string;
    private _clientId!: string;
    private _clientSecret!: string;

    get requestArguments(): string[] {
        return [
            'applicationId',
        ];
    }

    get properties(): string[] {
        return [
            'applicationId',
            'clientId',
            'clientSecret',
        ];
    }

    get applicationId(): string {
        return this._applicationId;
    }

    set applicationId(value: string) {
        this._applicationId = value;
        this.propertyChanged('applicationId');
    }
    get clientId(): string {
        return this._clientId;
    }

    set clientId(value: string) {
        this._clientId = value;
        this.propertyChanged('clientId');
    }
    get clientSecret(): string {
        return this._clientSecret;
    }

    set clientSecret(value: string) {
        this._clientSecret = value;
        this.propertyChanged('clientSecret');
    }

    static use(initialValues?: IConfigureAuthentication): [ConfigureAuthentication, SetCommandValues<IConfigureAuthentication>] {
        return useCommand<ConfigureAuthentication, IConfigureAuthentication>(ConfigureAuthentication, initialValues);
    }
}
