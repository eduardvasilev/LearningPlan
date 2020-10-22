import http from "../http-common";
import { store } from "../store/index";
import { Topic } from "../model/topic";

export class TopicDataService {
    public addTopic(topic: Topic) {
        return http.post('/topic', topic, { headers: { 'Authorization': store.state.user.token } });
    }
}

export default new TopicDataService();