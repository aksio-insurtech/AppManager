/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { Command, CommandValidator, CommandPropertyValidators, useCommand, SetCommandValues } from '@aksio/cratis-applications-frontend/commands';
import { Validator } from '@aksio/cratis-applications-frontend/validation';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/applications/{{applicationId}}/environments/{{environmentId}}/custom-domain');

export interface IAddCustomDomainToApplicationEnvironment {
    applicationId?: string;
    environmentId?: string;
    domain?: string;
    certificate?: string;
}

export class AddCustomDomainToApplicationEnvironmentValidator extends CommandValidator {
    readonly properties: CommandPropertyValidators = {
        applicationId: new Validator(),
        environmentId: new Validator(),
        domain: new Validator(),
        certificate: new Validator(),
    };
}

export class AddCustomDomainToApplicationEnvironment extends Command<IAddCustomDomainToApplicationEnvironment> implements IAddCustomDomainToApplicationEnvironment {
    readonly route: string = '/api/applications/{{applicationId}}/environments/{{environmentId}}/custom-domain';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;
    readonly validation: CommandValidator = new AddCustomDomainToApplicationEnvironmentValidator();

    private _applicationId!: string;
    private _environmentId!: string;
    private _domain!: string;
    private _certificate!: string;

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
            'domain',
            'certificate',
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
    get domain(): string {
        return this._domain;
    }

    set domain(value: string) {
        this._domain = value;
        this.propertyChanged('domain');
    }
    get certificate(): string {
        return this._certificate;
    }

    set certificate(value: string) {
        this._certificate = value;
        this.propertyChanged('certificate');
    }

    static use(initialValues?: IAddCustomDomainToApplicationEnvironment): [AddCustomDomainToApplicationEnvironment, SetCommandValues<IAddCustomDomainToApplicationEnvironment>] {
        return useCommand<AddCustomDomainToApplicationEnvironment, IAddCustomDomainToApplicationEnvironment>(AddCustomDomainToApplicationEnvironment, initialValues);
    }
}
