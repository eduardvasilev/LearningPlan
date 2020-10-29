import http from "../http-common";
import { Topic } from "../models/topic";

export class TopicDataService {
    public addTopic(topic: Topic) {
        return http.post("/topic", topic);
    }

    public deleteTopic(id: string) {
        return http.delete(`/topic/${id}`);
    }

    public update(topic: Topic) {
        return http.put("/topic", topic);
    }
}

export default new TopicDataService();