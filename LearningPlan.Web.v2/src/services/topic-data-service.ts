import http from "./create-request";

import type { Topic } from "@/models/topic";

export class TopicDataService {
    public addTopic(topic: Topic) {
        return http().post("/topic", topic);
    }

    public deleteTopic(id: string) {
        return http().delete(`/topic/${id}`);
    }

    public update(topic: Topic) {
        return http().put("/topic", topic);
    }
}

export default new TopicDataService();
