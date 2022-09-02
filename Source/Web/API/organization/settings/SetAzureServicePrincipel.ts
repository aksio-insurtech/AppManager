/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { Command, CommandValidator, CommandPropertyValidators, useCommand, SetCommandValues } from '@aksio/cratis-applications-frontend/commands';
import { Validator } from '@aksio/cratis-applications-frontend/validation';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/organization/settings');

export interface ISetAzureServicePrincipel {
    clientId?: string;
    clientSecret?: string;
}

export class SetAzureServicePrincipelValidator extends CommandValidator {
    readonly properties: CommandPropertyValidators = {
        clientId: new Validator(),
        clientSecret: new Validator(),
    };
}

export class SetAzureServicePrincipel extends Command<ISetAzureServicePrincipel> implements ISetAzureServicePrincipel {
    readonly route: string = '/api/organization/settings';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;
    readonly validation: CommandValidator = new SetAzureServicePrincipelValidator();

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

    static use(initialValues?: ISetAzureServicePrincipel): [SetAzureServicePrincipel, SetCommandValues<ISetAzureServicePrincipel>] {
        return useCommand<SetAzureServicePrincipel, ISetAzureServicePrincipel>(SetAzureServicePrincipel, initialValues);
    }
}
