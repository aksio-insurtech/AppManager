/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { Command, CommandValidator, CommandPropertyValidators, useCommand, SetCommandValues, ClearCommandValues } from '@aksio/applications/commands';
import { Validator } from '@aksio/applications/validation';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/applications/{{applicationId}}/environments/{{environmentId}}/microservices/{{microserviceId}}/app-settings');

export interface ISetAppSettingsForMicroservice {
    applicationId?: string;
    environmentId?: string;
    microserviceId?: string;
    content?: string;
}

export class SetAppSettingsForMicroserviceValidator extends CommandValidator {
    readonly properties: CommandPropertyValidators = {
        applicationId: new Validator(),
        environmentId: new Validator(),
        microserviceId: new Validator(),
        content: new Validator(),
    };
}

export class SetAppSettingsForMicroservice extends Command<ISetAppSettingsForMicroservice> implements ISetAppSettingsForMicroservice {
    readonly route: string = '/api/applications/{{applicationId}}/environments/{{environmentId}}/microservices/{{microserviceId}}/app-settings';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;
    readonly validation: CommandValidator = new SetAppSettingsForMicroserviceValidator();

    private _applicationId!: string;
    private _environmentId!: string;
    private _microserviceId!: string;
    private _content!: string;

    constructor() {
        super(Object, false);
    }

    get requestArguments(): string[] {
        return [
            'applicationId',
            'environmentId',
            'microserviceId',
        ];
    }

    get properties(): string[] {
        return [
            'applicationId',
            'environmentId',
            'microserviceId',
            'content',
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
    get microserviceId(): string {
        return this._microserviceId;
    }

    set microserviceId(value: string) {
        this._microserviceId = value;
        this.propertyChanged('microserviceId');
    }
    get content(): string {
        return this._content;
    }

    set content(value: string) {
        this._content = value;
        this.propertyChanged('content');
    }

    static use(initialValues?: ISetAppSettingsForMicroservice): [SetAppSettingsForMicroservice, SetCommandValues<ISetAppSettingsForMicroservice>, ClearCommandValues] {
        return useCommand<SetAppSettingsForMicroservice, ISetAppSettingsForMicroservice>(SetAppSettingsForMicroservice, initialValues);
    }
}
