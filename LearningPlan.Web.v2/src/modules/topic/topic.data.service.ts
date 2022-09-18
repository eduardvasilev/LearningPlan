import createRequest from "@/modules/httprequest/create-request.service";

import type { Topic } from "@/modules/topic/classes/topic.class";

export class TopicDataService {
    public addTopic(topic: Topic) {
        return createRequest().post("/topic", topic);
    }

    public deleteTopic(id: string) {
        return createRequest().delete(`/topic/${id}`);
    }

    public update(topic: Topic) {
        return createRequest().put("/topic", topic);
    }
}

export default new TopicDataService();
