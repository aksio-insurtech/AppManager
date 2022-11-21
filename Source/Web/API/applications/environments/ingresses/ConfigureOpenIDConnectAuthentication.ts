/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { Command, CommandValidator, CommandPropertyValidators, useCommand, SetCommandValues, ClearCommandValues } from '@aksio/cratis-applications-frontend/commands';
import { Validator } from '@aksio/cratis-applications-frontend/validation';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/applications/{{applicationId}}/environments/{{environmentId}}/ingresses/{{ingressId}}/authentication/openid');

export interface IConfigureOpenIDConnectAuthentication {
    applicationId?: string;
    environmentId?: string;
    ingressId?: string;
    clientId?: string;
    clientSecret?: string;
}

export class ConfigureOpenIDConnectAuthenticationValidator extends CommandValidator {
    readonly properties: CommandPropertyValidators = {
        applicationId: new Validator(),
        environmentId: new Validator(),
        ingressId: new Validator(),
        clientId: new Validator(),
        clientSecret: new Validator(),
    };
}

export class ConfigureOpenIDConnectAuthentication extends Command<IConfigureOpenIDConnectAuthentication> implements IConfigureOpenIDConnectAuthentication {
    readonly route: string = '/api/applications/{{applicationId}}/environments/{{environmentId}}/ingresses/{{ingressId}}/authentication/openid';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;
    readonly validation: CommandValidator = new ConfigureOpenIDConnectAuthenticationValidator();

    private _applicationId!: string;
    private _environmentId!: string;
    private _ingressId!: string;
    private _clientId!: string;
    private _clientSecret!: string;

    get requestArguments(): string[] {
        return [
            'applicationId',
            'environmentId',
            'ingressId',
        ];
    }

    get properties(): string[] {
        return [
            'applicationId',
            'environmentId',
            'ingressId',
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
    get environmentId(): string {
        return this._environmentId;
    }

    set environmentId(value: string) {
        this._environmentId = value;
        this.propertyChanged('environmentId');
    }
    get ingressId(): string {
        return this._ingressId;
    }

    set ingressId(value: string) {
        this._ingressId = value;
        this.propertyChanged('ingressId');
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

    static use(initialValues?: IConfigureOpenIDConnectAuthentication): [ConfigureOpenIDConnectAuthentication, SetCommandValues<IConfigureOpenIDConnectAuthentication>, ClearCommandValues] {
        return useCommand<ConfigureOpenIDConnectAuthentication, IConfigureOpenIDConnectAuthentication>(ConfigureOpenIDConnectAuthentication, initialValues);
    }
}
