/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { Command, CommandValidator, CommandPropertyValidators, useCommand, SetCommandValues, ClearCommandValues } from '@aksio/cratis-applications-frontend/commands';
import { Validator } from '@aksio/cratis-applications-frontend/validation';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/applications/{{applicationId}}/environments/{{environmentId}}/certificates');

export interface IAddCertificateToApplicationEnvironment {
    applicationId?: string;
    environmentId?: string;
    certificateId?: string;
    name?: string;
    certificate?: string;
}

export class AddCertificateToApplicationEnvironmentValidator extends CommandValidator {
    readonly properties: CommandPropertyValidators = {
        applicationId: new Validator(),
        environmentId: new Validator(),
        certificateId: new Validator(),
        name: new Validator(),
        certificate: new Validator(),
    };
}

export class AddCertificateToApplicationEnvironment extends Command<IAddCertificateToApplicationEnvironment> implements IAddCertificateToApplicationEnvironment {
    readonly route: string = '/api/applications/{{applicationId}}/environments/{{environmentId}}/certificates';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;
    readonly validation: CommandValidator = new AddCertificateToApplicationEnvironmentValidator();

    private _applicationId!: string;
    private _environmentId!: string;
    private _certificateId!: string;
    private _name!: string;
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
            'certificateId',
            'name',
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
    get certificateId(): string {
        return this._certificateId;
    }

    set certificateId(value: string) {
        this._certificateId = value;
        this.propertyChanged('certificateId');
    }
    get name(): string {
        return this._name;
    }

    set name(value: string) {
        this._name = value;
        this.propertyChanged('name');
    }
    get certificate(): string {
        return this._certificate;
    }

    set certificate(value: string) {
        this._certificate = value;
        this.propertyChanged('certificate');
    }

    static use(initialValues?: IAddCertificateToApplicationEnvironment): [AddCertificateToApplicationEnvironment, SetCommandValues<IAddCertificateToApplicationEnvironment>, ClearCommandValues] {
        return useCommand<AddCertificateToApplicationEnvironment, IAddCertificateToApplicationEnvironment>(AddCertificateToApplicationEnvironment, initialValues);
    }
}
