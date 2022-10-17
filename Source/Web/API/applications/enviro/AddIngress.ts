/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { Command, CommandValidator, CommandPropertyValidators, useCommand, SetCommandValues } from '@aksio/cratis-applications-frontend/commands';
import { Validator } from '@aksio/cratis-applications-frontend/validation';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/applications/{{applicationId}}/enviro{{environment}}/ingress');

export interface IAddIngress {
    applicationId?: string;
    environment?: string;
}

export class AddIngressValidator extends CommandValidator {
    readonly properties: CommandPropertyValidators = {
        applicationId: new Validator(),
        environment: new Validator(),
    };
}

export class AddIngress extends Command<IAddIngress> implements IAddIngress {
    readonly route: string = '/api/applications/{{applicationId}}/enviro{{environment}}/ingress';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;
    readonly validation: CommandValidator = new AddIngressValidator();

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

    static use(initialValues?: IAddIngress): [AddIngress, SetCommandValues<IAddIngress>] {
        return useCommand<AddIngress, IAddIngress>(AddIngress, initialValues);
    }
}
