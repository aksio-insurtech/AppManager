/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { Command, CommandValidator, CommandPropertyValidators, useCommand, SetCommandValues, ClearCommandValues } from '@aksio/cratis-applications-frontend/commands';
import { Validator } from '@aksio/cratis-applications-frontend/validation';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/applications/{{applicationId}}/environments/{{environmentId}}/microservices/{{microserviceId}}/remove');

export interface IRemove {
    applicationId?: string;
    environmentId?: string;
    microserviceId?: string;
}

export class RemoveValidator extends CommandValidator {
    readonly properties: CommandPropertyValidators = {
        applicationId: new Validator(),
        environmentId: new Validator(),
        microserviceId: new Validator(),
    };
}

export class Remove extends Command<IRemove> implements IRemove {
    readonly route: string = '/api/applications/{{applicationId}}/environments/{{environmentId}}/microservices/{{microserviceId}}/remove';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;
    readonly validation: CommandValidator = new RemoveValidator();

    private _applicationId!: string;
    private _environmentId!: string;
    private _microserviceId!: string;

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

    static use(initialValues?: IRemove): [Remove, SetCommandValues<IRemove>, ClearCommandValues] {
        return useCommand<Remove, IRemove>(Remove, initialValues);
    }
}
