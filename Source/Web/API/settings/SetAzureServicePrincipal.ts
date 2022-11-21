/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { Command, CommandValidator, CommandPropertyValidators, useCommand, SetCommandValues, ClearCommandValues } from '@aksio/cratis-applications-frontend/commands';
import { Validator } from '@aksio/cratis-applications-frontend/validation';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/settings/service-principal');

export interface ISetAzureServicePrincipal {
    clientId?: string;
    clientSecret?: string;
}

export class SetAzureServicePrincipalValidator extends CommandValidator {
    readonly properties: CommandPropertyValidators = {
        clientId: new Validator(),
        clientSecret: new Validator(),
    };
}

export class SetAzureServicePrincipal extends Command<ISetAzureServicePrincipal> implements ISetAzureServicePrincipal {
    readonly route: string = '/api/settings/service-principal';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;
    readonly validation: CommandValidator = new SetAzureServicePrincipalValidator();

    private _clientId!: string;
    private _clientSecret!: string;

    get requestArguments(): string[] {
        return [
        ];
    }

    get properties(): string[] {
        return [
            'clientId',
            'clientSecret',
        ];
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

    static use(initialValues?: ISetAzureServicePrincipal): [SetAzureServicePrincipal, SetCommandValues<ISetAzureServicePrincipal>, ClearCommandValues] {
        return useCommand<SetAzureServicePrincipal, ISetAzureServicePrincipal>(SetAzureServicePrincipal, initialValues);
    }
}
