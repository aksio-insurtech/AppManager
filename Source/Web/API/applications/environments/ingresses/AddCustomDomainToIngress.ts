/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { Command, CommandValidator, CommandPropertyValidators, useCommand, SetCommandValues } from '@aksio/cratis-applications-frontend/commands';
import { Validator } from '@aksio/cratis-applications-frontend/validation';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/applications/{{applicationId}}/environments/{{environmentId}}/ingresses/{ingressId}/custom-domain');

export interface IAddCustomDomainToIngress {
    applicationId?: string;
    environmentId?: string;
    domain?: string;
    certificateId?: string;
}

export class AddCustomDomainToIngressValidator extends CommandValidator {
    readonly properties: CommandPropertyValidators = {
        applicationId: new Validator(),
        environmentId: new Validator(),
        domain: new Validator(),
        certificateId: new Validator(),
    };
}

export class AddCustomDomainToIngress extends Command<IAddCustomDomainToIngress> implements IAddCustomDomainToIngress {
    readonly route: string = '/api/applications/{{applicationId}}/environments/{{environmentId}}/ingresses/{ingressId}/custom-domain';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;
    readonly validation: CommandValidator = new AddCustomDomainToIngressValidator();

    private _applicationId!: string;
    private _environmentId!: string;
    private _domain!: string;
    private _certificateId!: string;

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
            'certificateId',
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
    get certificateId(): string {
        return this._certificateId;
    }

    set certificateId(value: string) {
        this._certificateId = value;
        this.propertyChanged('certificateId');
    }

    static use(initialValues?: IAddCustomDomainToIngress): [AddCustomDomainToIngress, SetCommandValues<IAddCustomDomainToIngress>] {
        return useCommand<AddCustomDomainToIngress, IAddCustomDomainToIngress>(AddCustomDomainToIngress, initialValues);
    }
}