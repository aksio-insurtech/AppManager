/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { Command, CommandValidator, CommandPropertyValidators, useCommand, SetCommandValues, ClearCommandValues } from '@aksio/cratis-applications-frontend/commands';
import { Validator } from '@aksio/cratis-applications-frontend/validation';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/applications/{{applicationId}}/environments/{{environmentId}}/ingresses/{{ingressId}}/custom-domains');

export interface IAddCustomDomainToIngress {
    applicationId?: string;
    environmentId?: string;
    ingressId?: string;
    domain?: string;
    certificateId?: string;
}

export class AddCustomDomainToIngressValidator extends CommandValidator {
    readonly properties: CommandPropertyValidators = {
        applicationId: new Validator(),
        environmentId: new Validator(),
        ingressId: new Validator(),
        domain: new Validator(),
        certificateId: new Validator(),
    };
}

export class AddCustomDomainToIngress extends Command<IAddCustomDomainToIngress> implements IAddCustomDomainToIngress {
    readonly route: string = '/api/applications/{{applicationId}}/environments/{{environmentId}}/ingresses/{{ingressId}}/custom-domains';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;
    readonly validation: CommandValidator = new AddCustomDomainToIngressValidator();

    private _applicationId!: string;
    private _environmentId!: string;
    private _ingressId!: string;
    private _domain!: string;
    private _certificateId!: string;

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
    get ingressId(): string {
        return this._ingressId;
    }

    set ingressId(value: string) {
        this._ingressId = value;
        this.propertyChanged('ingressId');
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

    static use(initialValues?: IAddCustomDomainToIngress): [AddCustomDomainToIngress, SetCommandValues<IAddCustomDomainToIngress>, ClearCommandValues] {
        return useCommand<AddCustomDomainToIngress, IAddCustomDomainToIngress>(AddCustomDomainToIngress, initialValues);
    }
}
